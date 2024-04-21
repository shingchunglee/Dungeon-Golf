public enum FloorType
{
    EMPTY,
    WALL,
    OBSTACLE, //Similar to Wall except it represents an object that cannot be moved through
    FLOOR,
    VOID,
    TRAP_FLOOR, //Such as spikes, where you only get hurt if on the ground

}
