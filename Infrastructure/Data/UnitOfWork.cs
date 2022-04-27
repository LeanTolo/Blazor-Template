using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private ClientRepository _clientRepository;
        IDisposable _disposableResource = new MemoryStream();
        IAsyncDisposable _asyncDisposableResource = new MemoryStream();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IClientRepository ClientRepository => _clientRepository = _clientRepository ?? new ClientRepository(_context);


        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // siempre q lo use disposear los recursos, si voy a usar un metodo en el que tengo un objeto disposable y lo libero, mi clase no tiene q ser disposable,
        // en mi metodo creo el objeto disposable y lo libero, simplemente haciendo using

        // puede ser q mi objeto no comience y termine, y mi objeto necesite ser disposable, en ese caso tengo que implementar la clase idisposable y eso obliga implementarlo

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //Metodo quelibera recursos no administrados, realiza una limpieza general y le indica al garbage collector que no necesita ejecutarse
        //Por lo general, liberar la memoria asociada con un objeto administrado es trabajo del garbage collector
        public async ValueTask DisposeAsync()
        {
            // Async CleanUp - Limpieza asincronica
            await DisposeAsyncCore();
            // Dispose of Unmanaged resources - disponer de recursos no administrados
            Dispose(disposing: false);
            // Suppress Finalization - suprimir la finalizacion
            GC.SuppressFinalize(this);
        }

        //La Instancia IDisposable se elimina condicionalmente si no es nula y la IAsyncDisposable se
        //convierte en IDisposable, y si no es nula, tambien se elimina. Luego se asigna a ambas valor nulo
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposableResource?.Dispose();
                (_asyncDisposableResource as IDisposable)?.Dispose();
            }

            _disposableResource = null;
            _asyncDisposableResource = null;
        }

        //Realiza limpieza asincronica de recursos administrados o llama en cascada a DisposeAsync
        //Si la instancia IAsyncDisposable no es nula, se espera la configuracion(falsa). Si la instancia de IDisposable
        //es una implementacion de IAsyncDisposable, tambien se elimina de forma asincronica. Luego se asigna a ambas instancias valor nulo
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_asyncDisposableResource is not null)
            {
                await _asyncDisposableResource.DisposeAsync().ConfigureAwait(false);
            }

            if (_disposableResource is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                _disposableResource?.Dispose();
            }

            _asyncDisposableResource = null;
            _disposableResource = null;
        }
    }
}
