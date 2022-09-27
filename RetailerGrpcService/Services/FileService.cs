using Google.Protobuf;
using Grpc.Core;

namespace Retailer.GrpcService.Services
{
    public class FileService : FileGreeter.FileGreeterBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public override async Task FileDownload(FileInfo request, IServerStreamWriter<BytesContent> responseStream, ServerCallContext context)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "files");

            FileStream fileStream = new FileStream($"{filePath}/{request.FileName}{request.FileExtension}", FileMode.Open, FileAccess.Read);

            byte[] buffer = new byte[2048];

            BytesContent bytesContent = new BytesContent
            {
                FileSize = fileStream.Length,
                FileInfo = new FileInfo { FileName = Path.GetFileNameWithoutExtension(fileStream.Name), FileExtension = Path.GetExtension(fileStream.Name) },
                ReadedByte = 0,
            };

            while ((bytesContent.ReadedByte = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                bytesContent.Buffer = ByteString.CopyFrom(buffer);
                await responseStream.WriteAsync(bytesContent);
            }

            fileStream.Close();
        }

        public override async Task<EmptyMessage> FileUpload(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream? fileStream = null;

            try
            {
                int count = 0;

                while (await requestStream.MoveNext())
                {
                    if (count++ == 0)
                    {
                        fileStream = new FileStream($"{path}/{requestStream.Current.FileInfo.FileName.Append('z')}{requestStream.Current.FileInfo.FileExtension}", FileMode.CreateNew);

                        fileStream.SetLength(requestStream.Current.FileSize);
                    }

                    var buffer = requestStream.Current.Buffer.ToByteArray();

                    await fileStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
                throw;
            }

            await fileStream.DisposeAsync();

            fileStream.Close();

            return new EmptyMessage();
        }
    }
}
