using Pr0t0k07.APIsurdORM.Application.Workers;
using Pr0t0k07.APIsurdORM.Infrastructure.Shared.Services;


var generator = new GenerateApplication(null, 
    new FileService(null)) ;


await generator.Handle();