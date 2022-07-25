package learning_modules.java_exercises;

public class Movie {
    private String title;
    private String studio;
    private String rating;

    public Movie(String t, String s, String r){
       title=t;
       studio=s;
       rating=r;
    }

    public Movie(String t, String s){
        title=t;
        studio=s;
        rating="PG";
    }

    public static Movie[] getPG(Movie[] mov){
        Movie[] pgMov=new Movie[mov.length];
        int newArrayIndex=0;
        for ( int i= 0; i < mov.length; i++){
        if (mov[i].rating.equals("PG")){
            pgMov[newArrayIndex]=mov[i];
            newArrayIndex++;
        }
    }
    return pgMov;
    }
}
