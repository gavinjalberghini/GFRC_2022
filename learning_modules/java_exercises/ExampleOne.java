package learning_modules.java_exercises;
public class ExampleOne {

    public static void main(String[] args){
        int totalQ=65;
        double qValue=0.25;
        
        double totalQValue=totalQ*qValue;
int qRemainder=totalQ%3;
double valuePerFriend=(totalQ-qRemainder)*qValue/3;
double jaValue=valuePerFriend;
double joValue=valuePerFriend;
double joPlusJa=(joValue+jaValue);
double newJaTotal=joPlusJa+(qRemainder*qValue);

System.out.println("Total value: $" + totalQValue);
System.out.println("Remainder: " + qRemainder);
System.out.println("How much each friend has: $" + valuePerFriend);
System.out.println("New Jack value: $" + joPlusJa);
System.out.println("Final Jack value: $" + newJaTotal);
    }
} 