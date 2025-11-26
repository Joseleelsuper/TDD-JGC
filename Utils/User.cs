using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;

namespace Utils
{
     /*
      * Creo que TestLink no permite añadir o quitar idiomas, así que solo dejo los que yo decida que esten disponibles. 
      */
    public enum Language
    {
        ES,
        EN
    }

    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public string Surnames { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Language Language { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        private static int _idSeq = 0;

        public User(string username, string name, string surnames, string email, string rawPassword, Language language, bool isActive, DateTime expirationDate)
        {
            Id = NextId();
            Username = username;
            Name = name;
            Surnames = surnames;
            Email = email;
            Password = HashPassword(rawPassword);
            Language = language;
            IsActive = isActive;
            ExpirationDate = expirationDate;
        }

        private int NextId() => ++_idSeq;

        public bool SetUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) return false;
            // Solo minúsculas y dígitos
            foreach (var c in newUsername)
            {
                if (!(char.IsLower(c) || char.IsDigit(c))) return false;
            }
            Username = newUsername;
            return true;
        }

        /*
         * Ya que esta práctica no se basa en crear una BD, pondré este tipo de métodos dentro de la clase User.
        */
        public string HashPassword(string raw)
        {
            if (raw == null) return string.Empty;
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }

        public void SetName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName)) Name = newName;
        }

        public void SetSurnames(string newSurnames)
        {
            if (!string.IsNullOrWhiteSpace(newSurnames)) Surnames = newSurnames;
        }

        public bool SetEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail)) return false;
            try
            {
                var addr = new MailAddress(newEmail);
                var domain = addr.Host;
                if (string.IsNullOrWhiteSpace(domain) || !domain.Contains(".")) return false;
                Email = addr.Address;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetPassword(string newRawPassword)
        {
            if (string.IsNullOrWhiteSpace(newRawPassword)) return false;
            var newHash = HashPassword(newRawPassword);
            if (newHash == Password) return false;
            Password = newHash;
            return true;
        }

        public void SetLanguage(Language language)
        {
            Language = language;
        }

        public void DeactivateAccount()
        {
            IsActive = false;
        }

        public void ActivateAccount()
        {
            IsActive = true;
        }
    }
}