using Domain.ValueObject;

namespace Domain.Entities;

public abstract class Avaliacao 
{
    ///<summary>
    /// Uma avaliação terá um padrão de suportar até 5 fotos , um texto de 1000 caracteres 
    /// e uma avalização de 0 a 5  
    ///</summary> 
     
    public Guid Id { get; } 
    public Guid ProdutoId { get; set; } = Guid.Empty;
    public Guid ClienteId { get; set; } = Guid.Empty;
    public int Nota { get; set; }
    public string? Texto { get; set; }
    public ICollection<FotosAvaliacao> FotosAvaliacao { get; } = new List<FotosAvaliacao>();    
    public Avaliacao()
    {
        
    }
}