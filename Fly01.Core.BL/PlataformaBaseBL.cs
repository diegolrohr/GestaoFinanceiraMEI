using Fly01.Core.Entities.Domains;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fly01.Core.BL
{
    public class PlataformaBaseBL<TEntity> : DomainBaseBL<TEntity> where TEntity : PlataformaBase
    {
        private static readonly string[] _exceptions = new[] { "DataInclusao", "UsuarioInclusao" };
        private Expression<Func<TEntity, bool>> PredicatePlatform { get; set; }
        private string _plataformaUrl;
        public string PlataformaUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_plataformaUrl))
                    throw new BusinessException("ERRO!PlataformaUrl não informado.");

                return _plataformaUrl;
            }
            set
            {
                _plataformaUrl = value;
            }
        }

        protected RpcClient rpc { get; set; }

        private string _appUser;

        public string AppUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appUser))
                    throw new BusinessException("ERRO! AppUser não informado.");

                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }

        public bool MustConsumeMessageServiceBus { get; set; }

        public PlataformaBaseBL(AppDataContextBase context) : base(context)
        {
            PredicatePlatform = x => x.PlataformaId == context.PlataformaUrl && x.Ativo;
            AppUser = context.AppUser;
            PlataformaUrl = context.PlataformaUrl;
        }

        public override IQueryable<TEntity> All => base.All.Where(PredicatePlatform);

        public override IQueryable<TEntity> AllWithInactive => base.AllWithInactive.Where(x => x.PlataformaId == PlataformaUrl);

        public virtual IQueryable<TEntity> AllWithoutPlatform => base.repository.All;

        public override IQueryable<TEntity> AllWithInactiveIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return base.AllIncluding(includeProperties).Where(x => x.PlataformaId == PlataformaUrl);
        }

        public virtual void ValidaModel(TEntity entity)
        {
            if (!entity.IsValid())
                throw new BusinessException(entity.Notification.Get());
        }

        public virtual new void Insert(TEntity entity)
        {
            entity.PlataformaId = PlataformaUrl;
            entity.DataInclusao = DateTime.Now;
            entity.DataAlteracao = null;
            entity.DataExclusao = null;
            entity.UsuarioInclusao = AppUser;
            entity.UsuarioAlteracao = null;
            entity.UsuarioExclusao = null;
            if (!entity.Ativo)
            {
                entity.UsuarioExclusao = AppUser;
                entity.DataExclusao = DateTime.Now;
            }


            ValidaModel(entity);

            if (entity.Id == default(Guid) || entity.Id == null)
                entity.Id = Guid.NewGuid();

            repository.Insert(entity);
        }

        public virtual new void Update(TEntity entity)
        {
            entity.PlataformaId = PlataformaUrl;
            entity.DataAlteracao = DateTime.Now;
            entity.DataExclusao = null;
            entity.UsuarioAlteracao = AppUser;
            entity.UsuarioExclusao = null;
            if (!entity.Ativo)
            {
                entity.UsuarioExclusao = AppUser;
                entity.DataExclusao = DateTime.Now;
            }

            ValidaModel(entity);
        }

        public override void AttachForUpdate(TEntity entity)
        {
            var objectDB = Find(entity.Id);

            if (objectDB == null) return;

            entity.CopyProperties<TEntity>(objectDB, _exceptions);
        }

        public virtual new void Delete(TEntity entityToDelete)
        {
            entityToDelete.Ativo = false;
            entityToDelete.DataExclusao = DateTime.Now;
            entityToDelete.UsuarioExclusao = AppUser;

            repository.Delete(entityToDelete);
        }

        public virtual void Delete(Guid id) => Delete(Find(id));

        public override IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return base.AllIncluding(includeProperties).Where(PredicatePlatform);
        }

        /// <summary>
        /// Método responsável por realizar a persistência das mensagens trocadas entre os Apps.
        /// Caso seja necessário criar regras de negócio específicas para as entidades, este método deve ser sobrescrito.
        /// Este método será chamado sempre que o construtor da BL definir a propriedade 'isServiceBusRoute = true'.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="httpMethod"></param>
        public virtual void PersistMessage(dynamic item, RabbitConfig.EnHttpVerb httpMethod, bool isIntegracao)
        {
            if (!MustConsumeMessageServiceBus)
                return;

            switch (httpMethod)
            {
                case RabbitConfig.EnHttpVerb.POST:
                    {
                        var entity = JsonConvert.DeserializeObject<TEntity>(item.ToString());
                        if (entity == null) throw new Exception($"Não foi possível converter o objeto: {item.ToString()}");

                        Insert(entity);
                    }
                    break;
                case RabbitConfig.EnHttpVerb.PUT:
                    {
                        if (isIntegracao)
                        {
                            var id = item.Id ?? item.id;
                            var entity = Find(Guid.Parse(id.ToString()));
                            if (entity == null) throw new Exception($"Não foi possível editar o registro, pois o id {id.ToString()} não foi encontrado.");

                            var itemToUpdate = Reflection.CopyPropertiesFromJson(entity, item.ToString());

                            Update(itemToUpdate);
                            AttachForUpdate(itemToUpdate);
                        }
                        else
                        {
                            var itemToUpdate = JsonConvert.DeserializeObject<TEntity>(item.ToString());
                            if (itemToUpdate == null) throw new Exception($"Não foi possível editar o registro, pois o item {item.ToString()} não foi encontrado.");

                            Update(itemToUpdate);
                            AttachForUpdate(itemToUpdate);
                        }
                        break;
                    }
                case RabbitConfig.EnHttpVerb.DELETE:
                    {
                        var id = item.Id ?? item.id;
                        var itemToDelete = Find(Guid.Parse(id.ToString()));

                        if (itemToDelete == null) throw new Exception($"Não foi possível excluir o registro, pois o id {id.ToString()} não foi encontrado.");

                        Delete(itemToDelete);
                        AttachForUpdate(itemToDelete);
                        break;
                    }
            }
        }

        public virtual void AfterSave(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}