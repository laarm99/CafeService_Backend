using MultipleFormat.Application.Modules.PermissionsRequest.DTOs;

namespace NewProject.Application.Common.Storage
{
    public record StoredAttachment(string Id, string Name);

    // Guarda los adjuntos en disco: {root}\{formRequestId}\{guid}__{nombreOriginal}.
    // No requiere tablas; el Id publico es el nombre del archivo guardado.
    public class AttachmentStorage
    {
        public const long MaxFileBytes = 5 * 1024 * 1024;
        private const string Separator = "__";

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx"
        };

        private readonly string _root;

        public AttachmentStorage(string? rootPath)
        {
            _root = string.IsNullOrWhiteSpace(rootPath)
                ? Path.Combine(AppContext.BaseDirectory, "Uploads", "form-requests")
                : rootPath;
        }

        public string? Validate(IEnumerable<AttachmentDto>? files)
        {
            foreach (var file in files ?? Enumerable.Empty<AttachmentDto>())
            {
                var name = SafeName(file.FileName);
                if (file.Content == null || file.Content.Length == 0)
                {
                    return $"El archivo \"{name}\" está vacío.";
                }

                if (file.Content.Length > MaxFileBytes)
                {
                    return $"El archivo \"{name}\" excede 5 MB.";
                }

                if (!AllowedExtensions.Contains(Path.GetExtension(name)))
                {
                    return $"El tipo de archivo \"{name}\" no está permitido (PDF, JPG, PNG, DOC, DOCX).";
                }
            }

            return null;
        }

        public async Task SaveAsync(int formRequestId, IEnumerable<AttachmentDto>? files, CancellationToken cancellationToken)
        {
            var list = files?.Where(x => x.Content is { Length: > 0 }).ToList();
            if (list == null || list.Count == 0)
            {
                return;
            }

            var folder = FolderFor(formRequestId);
            Directory.CreateDirectory(folder);

            foreach (var file in list)
            {
                var stored = $"{Guid.NewGuid():N}{Separator}{SafeName(file.FileName)}";
                await File.WriteAllBytesAsync(Path.Combine(folder, stored), file.Content!, cancellationToken);
            }
        }

        public IReadOnlyList<StoredAttachment> List(int formRequestId)
        {
            var folder = FolderFor(formRequestId);
            if (!Directory.Exists(folder))
            {
                return Array.Empty<StoredAttachment>();
            }

            return Directory.EnumerateFiles(folder)
                .Select(Path.GetFileName)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new StoredAttachment(x!, OriginalName(x!)))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public (Stream Stream, string Name)? Open(int formRequestId, string fileId)
        {
            // Solo nombres planos: evita rutas relativas hacia otras carpetas.
            if (string.IsNullOrWhiteSpace(fileId) || fileId != Path.GetFileName(fileId))
            {
                return null;
            }

            var path = Path.Combine(FolderFor(formRequestId), fileId);
            if (!File.Exists(path))
            {
                return null;
            }

            return (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), OriginalName(fileId));
        }

        private string FolderFor(int formRequestId) => Path.Combine(_root, formRequestId.ToString());

        private static string OriginalName(string stored)
        {
            var index = stored.IndexOf(Separator, StringComparison.Ordinal);
            return index >= 0 ? stored[(index + Separator.Length)..] : stored;
        }

        private static string SafeName(string? fileName)
        {
            var name = Path.GetFileName(fileName ?? string.Empty);
            foreach (var invalid in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalid, '_');
            }

            return string.IsNullOrWhiteSpace(name) ? "archivo" : name;
        }
    }
}
