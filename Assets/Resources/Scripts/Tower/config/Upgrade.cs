// Machen Sie die Upgrade-Klasse abstrakt.
public abstract class Upgrade
{
    public string upgradeName;

    // Die Methode Apply muss von jeder Unterklasse implementiert werden.
    public abstract void Apply(Tower tower);
}
