using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;
using Hangfire.Common;
using Hangfire;
using MIT.ECSR.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MIT.ECSR.Core.Notification.Command
{

    #region Request
    public class AddNotificationMapping: Profile
    {
        public AddNotificationMapping()
        {
            CreateMap<AddNotificationRequest, NotificationRequest>().ReverseMap();
        }
    }
    public class AddNotificationRequest :NotificationRequest, IMapRequest<MIT.ECSR.Data.Model.TrsNotification, AddNotificationRequest>,IRequest<ObjectResponse<Guid>>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddNotificationRequest, MIT.ECSR.Data.Model.TrsNotification> map)
        {
            map.ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.Navigation, opt => opt.MapFrom(s => s.Navigation))
            .ForMember(d => d.Subject, opt => opt.MapFrom(s => s.Subject))
            .ForMember(d => d.IdUser, opt => opt.MapFrom(s => s.IdUser));
        }
    }
    #endregion

    internal class AddNotificationHandler : IRequestHandler<AddNotificationRequest, ObjectResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _job;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public AddNotificationHandler(
            ILogger<AddNotificationHandler> logger,
            IMapper mapper,
            IBackgroundJobClient job,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _job = job;
            _context = context;
        }
        public async Task<ObjectResponse<Guid>> Handle(AddNotificationRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<Guid> result = new ObjectResponse<Guid>();
            try
            {
                var data = _mapper.Map<Data.Model.TrsNotification>(request);
                data.IsOpen = false;
                data.CreateBy = request.Inputer;
                data.CreateDate = DateTime.Now;
                data.Id = Guid.NewGuid();
                var add = await _context.AddSave(data);
                if (add.Success)
                {
                    var user = await _context.Entity<SetUser>().Where(d => d.Id == data.IdUser).FirstOrDefaultAsync();
                    if (!string.IsNullOrWhiteSpace(user.Mail))
                    {
                        //_job.Enqueue(() =>
                        //                _mail.SendMail(
                        //                    new List<string>() { user.Mail }, null,
                        //                    request.Subject,
                        //                    request.Description,null
                        //                ));
                    }
                    result.Data = data.Id;
                    result.OK();
                }
                else
                    result.BadRequest(add.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add Notification", request);
                result.Error("Failed Add Notification", ex.Message);
            }
            return result;
        }
    }
}

