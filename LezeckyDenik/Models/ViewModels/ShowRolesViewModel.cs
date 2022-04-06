using Microsoft.AspNetCore.Mvc.Rendering;

namespace LezeckyDenik.Models.ViewModels
{
    //[Keyless]
    public class ShowRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        //[NotMapped]
        public IEnumerable<string> Roles { get; set; }

        public List<SelectListItem> ChooseRole { get; set; }

        public ShowRolesViewModel()
        {
            ChooseRole = new List<SelectListItem>();
            ChooseRole.Add(new SelectListItem { Text = Role.RoleSuperAdmin, Value = Role.RoleSuperAdmin });
            ChooseRole.Add(new SelectListItem { Text = Role.RoleAdmin, Value = Role.RoleAdmin });
            ChooseRole.Add(new SelectListItem { Text = Role.RoleEditor, Value = Role.RoleEditor });
            ChooseRole.Add(new SelectListItem { Text = Role.RoleUser, Value = Role.RoleUser });
        }
    }
}
