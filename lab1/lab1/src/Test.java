public class Test {

    public static void main(String[]args){
        Graph graph = new Graph();
        graph.initializeByFile("entryGraph.txt");

        BoruvkasAlgorithm boruvka = new BoruvkasAlgorithm();
        boruvka.boruvkaMST(graph);
    }
}
