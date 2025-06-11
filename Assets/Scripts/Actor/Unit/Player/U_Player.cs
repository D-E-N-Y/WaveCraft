
using UnityEngine;

public class U_Player : Unit
{
    public string proffesion { get; protected set; }
    private string[] names = {
        "Stigr Bosques", "Gaufrid Grosf", "Arnulf Färberg", "Herman Forstg", "Humbert Fishere",
        "Stigr Bognárh", "Robert Beutelg", "Ealhstan Fryee", "Hartmut Eichelg", "Cyneric Bradleye",
        "Hubert Beckete", "Gautbert Fiddlere", "Erwin Fieldse", "Hubert Berger", "Romilda Färberg",
        "Grimwald Fiddlere", "Ealdwine Fieldse", "Eadwulf Braung", "Fulbert Blumg", "Farvald Clineg",
        "Eadwulf Bognárh", "Aldric Fryee", "Anselm Froste", "Conrad Clineg", "Albert Bloms",
        "Gernot Eks", "Aldric Garverg", "Stígandr Bösch", "Stigr Faerberg", "Andebert Foxe",
        "Grimwald Forneyg", "Aldwin Frye"
    }; 

    [SerializeField] protected EVillageType type;
    public EVillageType Type() => type;

    public override string nameActor => unitName;
    private string unitName;

    public override void Initialize()
    {
        base.Initialize();

        unitName = names[Random.Range(0, names.Length)];

        VillageSystem.current.AddVillage(this);
    }
}