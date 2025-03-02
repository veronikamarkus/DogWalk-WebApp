// using App.Contracts.DAL.Repositories;
// using App.Domain;
// using Base.Contracts.DAL;
// using Base.DAL.EF;
//
// namespace App.DAL.EF.Repositories;
//
// public class MessageBoxRepository: BaseEntityRepository<MessageBox, MessageBox, AppDbContext>, IMessageBoxRepository
// {
//     public MessageBoxRepository(AppDbContext dbContext) : 
//         base(dbContext, new DalDomainMapper<MessageBox, MessageBox>())
//     {
//     }
// }