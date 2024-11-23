using System;
using System.Threading.Tasks; // Adicione esta diretiva
using Renci.SshNet;
using Microsoft.Extensions.Options;
namespace WebShell.Connection
{
    public class SshService : IDisposable
    {
        private SshClient _client;
        private bool _disposed = false;

        public SshService(IOptions<SshSettings> sshSettings) // Alterado para IOptions<SshSettings>
        {
            _client = new SshClient(sshSettings.Value.Host, sshSettings.Value.Username, sshSettings.Value.Password);

        }

        public async Task<string> ExecCommand(string command)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SshService));
            }

            if (!_client.IsConnected)
            {
                try
                {
                    _client.Connect();
                }
                catch (System.Net.Sockets.SocketException)
                {
                    return "[+] Falha ao se conectar com o servidor SSH.";
                }
            }

            return await Task.Run(() =>
            {
                var cmd = _client.CreateCommand(command);
                var output = cmd.Execute();
                return output;
            });
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
            _client.Dispose();
            _disposed = true;
        }
    }
}
