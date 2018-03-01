using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adultus.Helpers;
using Adultus.Models;

namespace Adultus.ViewModels
{
    public class LayoutViewModel
    {
        public Users User { get; set; }

        public List<Users> Users { get; set; }

        public List<Roles> Roles { get; set; }

        public List<Profiles> Profiles { get; set; }

        public List<ProfileRoles> ProfileRoles { get; set; }

        public LayoutViewModel LayoutViewModelBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Users> users = new List<Users>();
            users = SqlHelper.GetAllUsers();

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Users = users,
                Profiles = profiles,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelUserSearchBuilder(string profileId, string userId, string searchText)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Users> users = new List<Users>();
            users = SqlHelper.UserSearch(searchText);

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Users = users,
                Profiles = profiles,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelUserBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Profiles = profiles
            };
        }

        public LayoutViewModel LayoutViewModelUsersBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            List<Users> users = new List<Users>();
            users = SqlHelper.GetAllUsers();

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                Users = users,
                Profiles = profiles,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelAllProfileRolesBuilder(string userId)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetAllProfileRoles();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user
            };
        }

        public LayoutViewModel LayoutViewModelProfileRolesBuilder(string userId)
        {
            SqlHelper.DbContext();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetAllProfileRoles();

            List<Profiles> profiles = new List<Profiles>();
            profiles = SqlHelper.GetAllProfiles();

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Roles = roles,
                Profiles = profiles
            };
        }

        public LayoutViewModel LayoutViewModelRolesBuilder(string userId)
        {
            SqlHelper.DbContext();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(user.ProfileId);

            return new LayoutViewModel
            {
                User = user,
                Roles = roles,
                ProfileRoles = profileRoles
            };
        }

        public LayoutViewModel LayoutViewModelAllProfilesBuilder(string userId)
        {
            SqlHelper.DbContext();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);
            
            List<Profiles> profiles = new List<Profiles>();
            profiles = SqlHelper.GetAllProfiles();

            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(user.ProfileId);

            return new LayoutViewModel
            {
                User = user,
                Profiles = profiles,
                ProfileRoles = profileRoles
            };
        }
    }
}