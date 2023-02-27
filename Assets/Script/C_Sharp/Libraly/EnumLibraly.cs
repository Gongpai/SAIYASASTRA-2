//GameState
public enum GameState
{
    Play,
    Pause
}

//Dialog System
public enum SelectDialog
{
    name,
    dialog,
    note_title,
    note_text
}

//Set Active
public enum active
{
    True,
    False
}

//Fluorescent Tube SFX Play
public enum Fluorescent_Tube_SFX_Play
{
    SFX1, SFX2, SFX3, SFX4, SFX5
}

//Ai_Findding
public enum AiFindingMode
{
    FindingRandomLocation,
    FindingRandomTarget
}

//��Դ�յ�ҧ�
public enum AiGhost
{
    Hungry_ghost, //��������
    Home_ghost, //��ҷ��
    Guard_ghost, //����� + ��ѡ�ҹ 
    Kid_ghost, //����÷ͧ
    Mannequin_ghost, //��˭ԧ��ǻ��ȹ�
    Soi_Ju_ghost //������
}

//�к�����
public enum Use_Item_System
{
    Use_Self,
    Use_Other,
    Shoot_Projectile,
    Use_Light
}

//���͡˹������
public enum Essential_Menu
{
    Inventory,
    Craft,
    Note
}

public enum Object_interact
{
    Cupboard_Hide,
    Lawson_Door
}

public enum SystemMetric
{
    SM_CONVERTABLESLATEMODE = 0x2003,
    SM_SYSTEMDOCKED = 0x2004,
}

public enum ConvertibleMode 
{ 
    LaptopDockedMode, 
    SlateTabletMode 
}