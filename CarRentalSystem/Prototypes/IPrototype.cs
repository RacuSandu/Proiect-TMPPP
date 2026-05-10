namespace CarRentalSystem.Prototypes
{
    // Interfata Prototype - defineste metoda de clonare
    public interface IPrototype<T>
    {
        T ShallowCopy(); // copie superficiala
        T DeepCopy();    // copie profunda
    }
}