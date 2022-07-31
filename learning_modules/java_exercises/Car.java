package learning_modules.java_exercises;
/**
 * This is the Car subclass, where the requirements to make a variable of type Car are set.
 */
public class Car extends Vehicle{
    //The variable model is exclusive to the Car subclass
    public String model;
    /**
     * These are the requirements in order to create an object of type Car. 
     *They must have a brand, yearCreated, and model. 
     */
    public Car(String br, int yr, String mdl){
        //The variables brand and yearCreated come from the superclass Vehicle.
        brand=br;
        yearCreated=yr;
        //The variable model is made in this class.
        model=mdl;
        /*The mode variable is taken from the superclass Vehicles.
        It is used by both subclasses to announce how each type of vehicle moves. 
        Any object of type Car will have its mode set to drive.*/
        mode="drive";
    }
    
}
