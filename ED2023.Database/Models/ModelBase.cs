using ReactiveUI;

namespace ED2023.Database.Models; 

public class ModelBase : ReactiveObject, ICloneable {
    /// <returns>Не полный клон</returns>
    public virtual object Clone() {
        return new ModelBase();
    }
}