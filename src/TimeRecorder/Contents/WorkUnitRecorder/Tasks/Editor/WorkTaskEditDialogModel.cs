using System.Linq;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.Products;
using TimeRecorder.Domain.UseCase.WorkProcesses;
using TimeRecorder.UseCase.Clients;
using TimeRecorder.Domain.Domain.Segments;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor;

class WorkTaskEditDialogModel
{
    private readonly WorkProcessUseCase _ProcessUseCase;
    private readonly ClientUseCase _ClientUseCase;
    private readonly ProductUseCase _ProductUseCase;

    public WorkTaskEditDialogModel()
    {
        _ProcessUseCase = new WorkProcessUseCase(ContainerHelper.GetRequiredService<IWorkProcessRepository>());
        _ClientUseCase = new ClientUseCase(ContainerHelper.GetRequiredService<IClientRepository>());
        _ProductUseCase = new ProductUseCase(ContainerHelper.GetRequiredService<IProductRepository>());
    }

    public WorkProcess[] GetProcesses(Identity<WorkProcess> currentId)
    {
        return _ProcessUseCase.GetProcesses()
                              .Where(p => p.Id == currentId || p.Invalid == false)
                              .ToArray();
    }

    public Client[] GetClients()
    {
        var list = new List<Client> { Client.Empty };
        list.AddRange(_ClientUseCase.GetClients());
        return list.ToArray();
    }

    public Product[] GetProducts(Identity<Product> currentId)
    {
        var list = new List<Product> { Product.Empty };
        list.AddRange(_ProductUseCase.GetProducts().Where(p => p.Id == currentId || p.Invalid == false));
        return list.ToArray();
    }

    public Segment[] GetSegments()
    {
        var list = new List<Segment>();
        list.AddRange(ContainerHelper.GetRequiredService<ISegmentRepository>().SelectAll());
        return list.ToArray();
    }
}
