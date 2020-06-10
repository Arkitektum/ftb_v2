namespace FtB_PreProsessor.Repositories
{
    public interface IFormRepository
    {
        object GetForm(string reference);
        void Save(object form);
    }
}