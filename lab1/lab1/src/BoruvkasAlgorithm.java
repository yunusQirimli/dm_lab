public class BoruvkasAlgorithm {

    public void boruvkaMST(Graph graph) {

        //Getting data of given Graph
        int vertNum = graph.getVertNum();
        int edgeNum = graph.getEdgeNum();
        String[] vertNames = graph.getVertNames();
        Edge[] edges = graph.getEdges();

        Subset[] subsets = new Subset[vertNum];
        int[] cheapest = new int[vertNum];

        for (int i = 0; i < vertNum; i++) {
            subsets[i] = new Subset();
            subsets[i].setParent(i);
            subsets[i].setRank(0);
            cheapest[i] = -1;
        }

        //Initially the number or trees is equal to the
        // number of vertices and the MST weight is 0
        int numTree = vertNum;
        int MSTweight = 0;

        System.out.println("Initializing Boruvka's MST");

        //It will run until the MST is the only tree left
        while (numTree > 1) {
            System.out.println("Number of Vertices:" + numTree);

            //Reset the cheapest values every iteration
            for (int i = 0; i < vertNum; i++) {
                cheapest[i] = -1;
            }

            //Iterate over all edges to find the cheapest
            //edge of every subtree
            for (int i = 0; i < edgeNum; i++) {

                //Find the subsets of the corners of the edge
                int set1 = find(subsets, edges[i].getSrc());
                int set2 = find(subsets, edges[i].getDest());

                //If the two corners belong to the same subset,
                //ignore the current edge
                if (set1 != set2) {

                    //If they belong to different subsets, check which
                    //one is the cheapest
                    if (cheapest[set1] == -1 || edges[cheapest[set1]].getWeight() > edges[i].getWeight()) {
                        cheapest[set1] = i;
                    }

                    if (cheapest[set2] == -1 || edges[cheapest[set2]].getWeight() > edges[i].getWeight()) {
                        cheapest[set2] = i;
                    }
                }
            }

            //Add the cheapest edges obtained above to the MST
            for (int j = 0; j < vertNum; j++) {

                //Check if the cheapest for current set exists
                if (cheapest[j] != -1) {
                    int set1 = find(subsets, edges[cheapest[j]].getSrc());
                    int set2 = find(subsets, edges[cheapest[j]].getDest());

                    if(set1 != set2){
                        MSTweight += edges[cheapest[j]].getWeight();
                        System.out.println("Edge ("+ vertNames[edges[cheapest[j]].getSrc()] + ", " + vertNames[edges[cheapest[j]].getDest()]+") added to the MST");
                        uniteSubsets(subsets, set1, set2);
                        numTree--;
                    }
                }
            }
        }

        System.out.println("Final weight of MST :" + MSTweight);
    }

    //Method to find the set of a vert
    //It utilizes path compression technique
    private int find(Subset[] subsets, int vert) {
        if (subsets[vert].getParent() != vert) {
            subsets[vert].setParent(find(subsets, subsets[vert].getParent()));
        }
        return subsets[vert].getParent();
    }

    //Method to unite subsets, it uses the rank to select the parent
    private void uniteSubsets(Subset[] subsets, int v1, int v2){

        int rootv1 = find(subsets, v1);
        int rootv2 = find(subsets, v2);

        if(subsets[v1].getRank() < subsets[v2].getRank()){
            subsets[v1].setParent(subsets[v2].getParent());
        }else if(subsets[v1].getRank() > subsets[v2].getRank()){
            subsets[v2].setParent(subsets[v1].getParent());
        }else{
            subsets[v2].setParent(subsets[v1].getParent());
            subsets[v1].setRank(subsets[v1].getRank() + 1);
        }

    }
}
