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
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

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

    public void PutClient(Client client) => _ClientUseCase.Add(client);

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

    public async Task<Client[]> GetClientSourceAsync(string url)
    {
        //string url = "https://docs.google.com/spreadsheets/d/xxxxx/edit?usp=sharing";
        Regex regex = new Regex(@"https://docs\.google\.com/spreadsheets/d/(.*?)/(.*?)");

        Match match = regex.Match(url);
        if (match.Success == false)
        {
            return new Client[0];
        }

        var key = match.Groups[1].Value;
        //var key = "1UEMXvPblRBGzjBcaIvcGCO_1k0i5KP0ONwRZms0PV8w";
        var list = await ContainerHelper.GetRequiredService<IClientSourceRepository>().SelectByTaskCategoryAsync(key, Domain.Domain.Tasks.TaskCategory.Introduce);

        return list;
    }
}
