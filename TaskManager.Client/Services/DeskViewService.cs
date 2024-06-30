using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Client.Views.AddWindows;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services
{
    public class DeskViewService
    {
        private AuthToken _token;
        private DesksRequestService _desksRequestService;
        private CommonViewService _viewService;
        public DeskViewService(AuthToken token, DesksRequestService desksRequestService)
        {
            _token = token;
            _desksRequestService = desksRequestService;
            _viewService = new CommonViewService();
        }

        public ModelClient<DeskModel> GetDeskClientById(object deskId)
        {
            try
            {
                int id = (int)deskId;
                DeskModel desk = _desksRequestService.GetDeskById(_token, id);
                return new ModelClient<DeskModel>(desk);
            }
            catch (FormatException)
            {
                return new ModelClient<DeskModel>(null);
            }
        }

        public List<ModelClient<DeskModel>> GetDesks(int projectId)
        {
            return _desksRequestService.GetDesksByProject(_token, projectId).Select(d => new ModelClient<DeskModel>(d)).ToList();
        }

        public List<ModelClient<DeskModel>> GetAllDesks()
        {
            return _desksRequestService.GetAllDesks(_token).Select(d => new ModelClient<DeskModel>(d)).ToList();
        }

        public void OpenViewDeskInfo(object deskId, BindableBase context)
        {
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, context);
        }

        public void UpdateDesk(DeskModel desk)
        {
            var result = _desksRequestService.UpdateDesk(_token, desk);
            _viewService.ShowActionResult(result, "Доска обновлена");
            _viewService.CurrentOpenedWindow?.Close();
        }

        public void DeleteDesk(int deskId)
        {
            var result = _desksRequestService.DeleteDesk(_token, deskId);
            _viewService.ShowActionResult(result, "Доска удалена");
        }

        public ModelClient<DeskModel> SelectPhotoForDesk(ModelClient<DeskModel> selectedDesk)
        {
            if (selectedDesk?.Model != null)
            {
                _viewService.SetPhotoForObject(selectedDesk.Model);
                selectedDesk = new ModelClient<DeskModel>(selectedDesk.Model);
            }
            return selectedDesk;
        }

    }
}
