public interface ICanTeam
{
    // 0 is neutral, 1 is team 1, 2 is team 2, etc.
    // I do not know how to enforce and guarantee this, so hopefully
    // I always adhere to this and no issues arises as a result.
    void SetTeam( int team );
    int GetTeam();
}