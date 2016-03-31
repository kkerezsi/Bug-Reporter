using BugReporter.Bll.Repositories.Contracts;
using BugReporter.Models.Exceptions;
using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugReporter.Bll.Client;
using BugReporter.Models.Enums;

namespace BugReporter.Bll.Server.Components
{
    public class ServerImplementation : IBugReporterServer
    {
        private IUserRepository _userRepository;
        private IReportsRepository _reportsRepository;

        private readonly IDictionary<String, IBugReporterObserver> _loggedClients;

        public ServerImplementation(IUserRepository userRepo, IReportsRepository reportsRepo)
        {
            _userRepository = userRepo;
            _reportsRepository = reportsRepo;
            _loggedClients = new Dictionary<String, IBugReporterObserver>();
        }

        public User Login(User user, IBugReporterObserver client)
        {
            var result = _userRepository.GetUser(user);
            if (result != null)
            {
                if (_loggedClients.ContainsKey(result.Username))
                    throw new ConnectionException("User already logged in.");
                _loggedClients[result.Username] = client;

                return result;
            }
            else
                throw new ConnectionException("Authentication failed.");
        }

        public void Logout(User user, IBugReporterObserver client)
        {
            IBugReporterObserver localClient = _loggedClients[user.Username];
            if (localClient == null)
                throw new ConnectionException("User " + user.Username + " is not logged in.");
            _loggedClients.Remove(user.Username);
        }

        public ReportModel GetReports(IBugReporterObserver client)
        {
            var result = _reportsRepository.GetReports();
            if (result != null)
            {
                return result;
            }
            else
                throw new ConnectionException("Null reports repository.");
        }

        public int SaveReports(ReportModel reportModel)
        {
            _reportsRepository.SaveReports(reportModel);

            foreach (var client in _loggedClients)
            {
                client.Value.UpdateReports(reportModel);
            }

            return 0;
        }
    }
}
