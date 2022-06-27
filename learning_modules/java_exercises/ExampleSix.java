package learning_modules.java_exercises;

import java.util.ArrayList;

public class ExampleSix {
    
    public static void main(String[] args){
        ArrayList<Double> list=new ArrayList<Double>();
        
        list.add(4.3);
        list.add(27.8);
        list.add(1.0);
        list.add(3.45);
        list.add(99.99);
        
        System.out.println(list.size());

        list.add(3, 2.5);
        list.remove(27.8);
        list.set(0, 5.55);

        for(double num: list){
            System.out.println(num);
        }

    }
}