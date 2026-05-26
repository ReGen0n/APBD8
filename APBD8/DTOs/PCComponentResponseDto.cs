namespace APBD8.DTOs;

public class PCComponentResponseDto
{
    public int Amount { get; set; }
    public ComponentResponseDto Component { get; set; } = null!;
}