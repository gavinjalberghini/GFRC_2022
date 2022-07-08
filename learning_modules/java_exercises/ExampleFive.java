package learning_modules.java_exercises;

public class ExampleFive {
    public static void main(String[] args){
    
        String[] names={"cats","dogs","birds","turtles","frogs"};
        int[] amount={5,3,10,2,4};

        int x=0;
        while(x<5){
            System.out.println( amount[x] + " " + names[x]);
            x++;
        }
        names[3]="ferrets";
        
        for(int y=0; y<=4; y++){
            System.out.println( amount[y] + " " + names[y]);
        }
    
}
}
