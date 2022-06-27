package learning_modules.java_exercises;

public class ExampleTwo {
    public static void main(String[] args){
        int currentYear=781;
        boolean leapYear=false;

        if(currentYear%4==0)
        {if(currentYear%100==0)
        {if(currentYear%400==0)
        {leapYear=true;
        }
        else{leapYear=false;
        }
        }
        else{leapYear=true;
        }
        }
        else{ leapYear=false;
        }
    

        if (leapYear=true)
        {
            System.out.println(currentYear + " is a leap year");
        }
        else{

        System.out.println(currentYear + " is not a leap year");
        }
    }
}