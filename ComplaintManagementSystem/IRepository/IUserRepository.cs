using ComplaintManagementSystem.Models;

namespace ComplaintManagementSystem.IRepository
{
    public interface IUserRepository
    {
        void CreateUser(UserModel user);

        UserModel ValidateUser(UserModel model);

        IEnumerable<ComplaintModel> GetComplaintByUserId(string user, string Column, string value, int StartIndex, int count);

        void CreateComplaint(ComplaintModel complaint);

        IEnumerable<ComplaintModel> GetComplaints(string Column, string value, int StartIndex, int count);

        void DeleteComplaint(int ids);

        void UpdateComplaint(ComplaintModel complaint);
    }
}
