using KR_1.Domain.Visitors;

namespace KR_1.Domain;

public interface IExportable
{
    void Accept(IExportVisitor visitor);
}