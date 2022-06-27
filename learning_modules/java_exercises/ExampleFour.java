package learning_modules.java_exercises;

import javax.xml.catalog.CatalogResolver;

public class ExampleFour {
    public static void main(String[] args){
        String[] names={"cats","dogs","birds","turtles","frogs"};
        int[] amount={5,3,10,2,4};

        System.out.println("We have: ");
    
        System.out.println( amount[0] + " " + names[0]);
        System.out.println(amount[1] + " " + names[1]);
        System.out.println(amount[2] + " " + names[2]);
        System.out.println(amount[3] + " " + names[3]);
        System.out.println(amount[4] + " " + names[4]);
        System.out.println("in stock");

        names[3]="ferrets";

        System.out.println("We have: ");
        System.out.println(amount[0] + " " + names[0]);
        System.out.println(amount[1] + " " + names[1]);
        System.out.println(amount[2] + " " + names[2]);
        System.out.println(amount[3] + " " + names[3]);
        System.out.println(amount[4] + " " + names[4]);
        System.out.println("in stock");

}
}