package learning_modules.java_exercises;
/**
 * This class has a few purposes.
 * It creates an object of type Car called car1 with  brand Honda, yearCreated 2009, and model CRV.
 * It also creates an object of type Airplane called plane1 with brand Boeing, yearCreated 2000, and id 717.
 * Finally, it prints all the relevant variables related to the object car1 and plane1.
 * The brand, yearCreated, and mode are printed for both vehicles. 
 * The model is also printed for the car and the id is printed for the plane.
 * I added a couple things just to make this look nicer when printed.
 * The line car 1, the empty line, and the line plane 1 were all extra parts added by me .
 */
public class VehiclePrinters {
    public static void main(String[] args) throws Exception {
        
        Car car1 = new Car("Honda", 2009, "CRV");

        System.out.println("Car 1");
        System.out.println("Brand: " + car1.brand);
        System.out.println("Year: " + car1.yearCreated);
        System.out.println("Model: " + car1.model);
        System.out.println("Mode: " + car1.mode);

        System.out.println(" ");

        Airplane plane1 = new Airplane("Boeing", 2000, 717);

        System.out.println("Plane 1");
        System.out.println("Brand: " + plane1.brand);
        System.out.println("Year: " + plane1.yearCreated);
        System.out.println("ID: " + plane1.id);
        System.out.println("Mode: " + plane1.mode);

    }
}
