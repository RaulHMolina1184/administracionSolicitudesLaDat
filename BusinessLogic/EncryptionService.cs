using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;
using System.Security.Cryptography;

namespace BusinessLogic
{
    public class EncryptionService
    {
        private readonly byte[] _key;
        private const int NonceSize = 12; // Tamaño estándar para nonce en GCM
        private const int TagSize = 16;   // Tamaño estándar para etiqueta de autenticación (128 bits)

        public EncryptionService(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("La clave no puede estar vacía.");

            _key = Encoding.UTF8.GetBytes(key);

            if (_key.Length != 32)
                throw new ArgumentException("Error de longitud para: AES-256-GCM.");
        }

        // Para encriptar datos
        public byte[] Encrypt(byte[] plaintext, byte[] associatedData, out byte[] nonce)
        {
            nonce = new byte[NonceSize];
            new SecureRandom().NextBytes(nonce);

            GcmBlockCipher cipher = new GcmBlockCipher(new Org.BouncyCastle.Crypto.Engines.AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(_key), TagSize * 8, nonce, associatedData);

            cipher.Init(true, parameters);

            byte[] ciphertext = new byte[cipher.GetOutputSize(plaintext.Length)];
            int len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
            cipher.DoFinal(ciphertext, len);

            return ciphertext;
        }

        // Para desencriptar datos
        public string Decrypt(byte[] ciphertext, byte[] nonce)
        {
            var cipher = new GcmBlockCipher(new Org.BouncyCastle.Crypto.Engines.AesEngine());
            var parameters = new AeadParameters(new KeyParameter(_key), TagSize * 8, nonce);

            cipher.Init(false, parameters);

            byte[] plaintext = new byte[cipher.GetOutputSize(ciphertext.Length)];
            int len = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plaintext, 0);
            cipher.DoFinal(plaintext, len);

            return Encoding.UTF8.GetString(plaintext);
        }

        // Para verificar credenciales de usuario
        public bool VerifyPassword(byte[] encryptedPassword, byte[] nonce, string passwordInput)
        {
            try
            {
                string decryptedPassword = Decrypt(encryptedPassword, nonce);

                byte[] inputBytes = Encoding.UTF8.GetBytes(passwordInput);
                byte[] decryptedBytes = Encoding.UTF8.GetBytes(decryptedPassword);

                return FixedTimeEquals(decryptedBytes, inputBytes);
            }
            catch
            {
                return false;
            }
        }

        private bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }


        // Para registrar nuevo usuario

        /* 
        public void RegistrarUsuario(string idUsuario, string nombreUsuario, string nombreCompleto, string correo, string password)
        {
            byte[] key = Encoding.UTF8.GetBytes("clave de 32 caracteres aquí!!!!"); // 32 bytes exactos
            byte[] plaintext = Encoding.UTF8.GetBytes(password);
            byte[] nonce;

            byte[] ciphertext = AesGcmBouncyCastle.Encrypt(key, plaintext, null, out nonce);

            using (var conn = new SqlConnection("tu_cadena_conexion"))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            INSERT INTO usuarios (id_usuario, nombre_usuario, nombre_completo, correo_electronico, contrasenia_cifrada, nonce, estado)
            VALUES (@id, @nombre_usuario, @nombre_completo, @correo, @contrasenia_cifrada, @nonce, 'activo')", conn);

                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.Parameters.AddWithValue("@nombre_usuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@nombre_completo", nombreCompleto);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contrasenia_cifrada", ciphertext);
                cmd.Parameters.AddWithValue("@nonce", nonce);

                cmd.ExecuteNonQuery();
            }
        }
        */
    }
}
