package learning_modules.java_exercises;

public class ExampleThree {
    public static void main(String[] args){
    String currentDay="thursday";
    
    switch(currentDay){
        case "monday":
            System.out.println("5 days until the weekend");
             break;
        case "tuesday":
            System.out.println("4 days until the weekend");
            break;
        case "wenesday":
            System.out.println("3 days until the weekend");
            break;
        case "thursday":
            System.out.println("2 days until the weekend");
            break;
        case "friday":
            System.out.println("1 days until the weekend");
            break;
        default:
            System.out.println("You've made it to the weekend!");
            break;
    }

    int month=9;
    String stMonth="January";

    switch(month){
        case 1:
            stMonth="January";
            break;
        case 2:
            stMonth="February";
            break;
        case 3:
            stMonth="March";
            break;
        case 4:
            stMonth="April";
            break;
        case 5:
            stMonth="May";
            break;
        case 6:
            stMonth="June";
            break;
        case 7:
            stMonth="July";
            break;
        case 8:
            stMonth="August";
            break;
        case 9:
            stMonth="September";
            break;
        case 10:
            stMonth="October";
            break;
        case 11:
            stMonth="November";
            break;
        case 12:
            stMonth="December";
            break;
    }
    System.out.println("This month is : " + stMonth);
}
}