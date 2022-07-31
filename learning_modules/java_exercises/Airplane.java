package learning_modules.java_exercises;
/**
 * This is the Airplane subclass, where the requirements to make a variable of type Airplane are set.
 */
public class Airplane extends Vehicle{
    //The variable id is exclusive to the Airplane subclass
    public int id;
    /**
     * These are the requirements in order to create an object of type Airplane. 
     *They must have a brand, yearCreated, and id.
     */
    public Airplane(String br, int yr, int i){
        //The variables brand and yearCreated come from the superclass Vehicle.
        brand=br;
        yearCreated=yr;
        //The variable id is made in this class.
        id=i;
        /*The mode variable is taken from the superclass Vehicles.
        It is used by both subclasses to announce how each type of vehicle moves. 
        Any object of type Airplane will have its mode set to fly.*/
        mode="fly";
    }
  
    
}
