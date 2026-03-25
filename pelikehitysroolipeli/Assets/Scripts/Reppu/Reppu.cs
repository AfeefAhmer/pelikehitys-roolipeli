using System.Collections.Generic;
using System.Linq;

public class Reppu
{
    private List<Tavara> tavarat = new List<Tavara>();

    public int MaxMaara { get; }
    public float MaxPaino { get; }
    public float MaxTilavuus { get; }

    public Reppu(int maxMaara, float maxPaino, float maxTilavuus)
    {
        MaxMaara = maxMaara;
        MaxPaino = maxPaino;
        MaxTilavuus = maxTilavuus;
    }

    public int TavaroidenMaara => tavarat.Count;
    public float NykyPaino => tavarat.Sum(t => t.Paino);
    public float NykyTilavuus => tavarat.Sum(t => t.Tilavuus);

    public List<Tavara> GetItems() => tavarat;

    public bool Lisaa(Tavara tavara)
    {
        if (tavarat.Count >= MaxMaara) return false;
        if (NykyPaino + tavara.Paino > MaxPaino) return false;
        if (NykyTilavuus + tavara.Tilavuus > MaxTilavuus) return false;

        tavarat.Add(tavara);
        return true;
    }
}