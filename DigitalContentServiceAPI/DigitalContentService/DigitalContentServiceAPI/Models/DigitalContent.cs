namespace DigitalContentServiceAPI.Models;

public abstract class DigitalContent
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PartitionKey { get; set; } = "DigitalContent";
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public bool ActiveContent { get; set; } = true;
    public string CreatedBy { get; set; }  = Guid.NewGuid().ToString();

    public void ChangeArchiveStateAsync(bool activeContent)
    {
        if (activeContent)
             activeContent = false;
        else 
            activeContent = true;
    }
}