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
    dialog
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

public enum AiGhost
{
    Hungry_ghost, //��������
    Home_ghost, //��ҷ��
    Guard_ghost, //����� + ��ѡ�ҹ 
    Kid_ghost, //����÷ͧ
    Woman_ghost, //��˭ԧ��ǻ��ȹ�
    Soi_Ju_ghost //������
}