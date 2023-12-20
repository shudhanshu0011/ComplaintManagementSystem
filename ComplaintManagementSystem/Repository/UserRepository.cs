using ComplaintManagementSystem.IRepository;
using ComplaintManagementSystem.Models;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Crypto;
using System.Data;
using WebCalculator.DapperContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComplaintManagementSystem.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DapperContext context;

        public UserRepository(DapperContext context)
        {
            this.context = context;
        }

        public void CreateUser(UserModel user)
        {
            using (var connection = context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("FirstName", user.FirstName);
                parameters.Add("LastName", user.LastName);
                parameters.Add("Email", user.Email);
                parameters.Add("Passwords", user.Passwords);
                parameters.Add("UserName", user.UserName);
                connection.Execute("CreateUser", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void CreateComplaint(ComplaintModel complaint)
        {
            using (var connection = context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("ComplaintDate", complaint.ComplaintDate);
                parameters.Add("ComplaintUserId", complaint.ComplaintUserId);
                parameters.Add("ComplaintTopic", complaint.ComplaintTopic);
                parameters.Add("ComplaintAddress", complaint.ComplaintAddress);
                parameters.Add("ComplaintDesc", complaint.ComplaintDesc);
                connection.Execute("CreateComplaint", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteComplaint(int ids)
        {
            using (var connection = context.CreateConnection())
            {
                connection.Execute("DeleteComplaint", new { ids }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void UpdateComplaint(ComplaintModel complaint)
        {
            using (var connection = context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("ComplaintDate", complaint.ComplaintDate);
                parameters.Add("ComplaintTopic", complaint.ComplaintTopic);
                parameters.Add("ComplaintAddress", complaint.ComplaintAddress);
                parameters.Add("ComplaintDesc", complaint.ComplaintDesc);
                parameters.Add("ComplaintIds", complaint.ComplaintId);
                connection.Execute("UpdateComplaint", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public UserModel ValidateUser(UserModel model)
        {
            using(var connection = context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("UserNames", model.UserName);
                parameters.Add("UserPasswords", model.Passwords);
                UserModel data = connection.QueryFirstOrDefault<UserModel>("ValidateUser", parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public IEnumerable<ComplaintModel> GetComplaintByUserId(string user, string SearchType, string SearchValue, int StartIndex, int count)
        {
            using (var connection = context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("ComplaintUserIds", user);
                IEnumerable<ComplaintModel> Data = connection.Query<ComplaintModel>("GetComplaintByUserId", parameters, commandType: CommandType.StoredProcedure);
                count = Data.Count();
                return count > 0
                    ? Data.Skip(StartIndex).Take(count).ToList()
                    : Data.ToList();
            }
        }

        public IEnumerable<ComplaintModel> GetComplaints(string SearchType, string SearchValue, int StartIndex, int count)
        {
            using (var connection = context.CreateConnection())
            {
                IEnumerable<ComplaintModel> Data;
                var parameters = new DynamicParameters();
                switch (SearchType)
                {
                    case "AllComplaints":
                        Data = connection.Query<ComplaintModel>("GetAllComplaint", commandType: CommandType.StoredProcedure).ToList();
                        count = Data.Count();
                        break;

                    case "ComplaintUserId":
                        parameters.Add("ComplaintUserIds", SearchValue);
                        Data = connection.Query<ComplaintModel>("GetComplaintByUserId", parameters, commandType: CommandType.StoredProcedure).ToList();
                        count = Data.Count();
                        break;

                    case "ComplaintId":
                        parameters.Add("ComplaintIds", SearchValue);
                        Data = connection.Query<ComplaintModel>("GetComplaintByComplaintId", parameters, commandType: CommandType.StoredProcedure).ToList();
                        count = Data.Count();
                        break;

                    case "ComplaintAddress":
                        parameters.Add("ComplaintAddresss", SearchValue);
                        Data = connection.Query<ComplaintModel>("GetComplaintByAddress", parameters, commandType: CommandType.StoredProcedure).ToList();
                        count = Data.Count();
                        break;

                    default:
                        Data = connection.Query<ComplaintModel>("GetAllComplaint", commandType: CommandType.StoredProcedure).ToList();
                        break;
                }
                return Data;
            }
        }
    } 
}
