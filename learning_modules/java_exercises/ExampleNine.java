package learning_modules.java_exercises;

public class ExampleNine {
    public static void main(String[] args){
    String ogSentence= "The quick brown fox jumbs over the lazy dog";
    System.out.println(ogSentence.length());
    System.out.println(ogSentence.substring(31));
    System.out.println(ogSentence.compareToIgnoreCase("The lazy dog jumps over the quick brown fox"));
    String[] words=ogSentence.split(" ");
    System.out.println(words.length);
    }
}
