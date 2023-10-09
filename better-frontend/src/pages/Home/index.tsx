import { useQuery } from "@tanstack/react-query";
import { getQuizzes } from "../../api/quizzes";
import { Spinner } from "@chakra-ui/spinner";
import { Card, CardHeader } from "@chakra-ui/card";
import { SimpleGrid } from "@chakra-ui/layout";
import { Link as ReactRouterLink } from "react-router-dom";
function Home() {
  const { isLoading, data, isError } = useQuery({
    queryKey: ["quizzes"],
    queryFn: getQuizzes,
  });

  if (isLoading) {
    return <Spinner />;
  }

  if (isError) {
    return "Unexpected Error!";
  }

  return (
    <SimpleGrid gap="2" templateColumns="repeat(auto-fit, minmax(240px, 1fr))">
      {data.map((quiz) => (
        <ReactRouterLink to={`/quizzes/${quiz.id}`} key={quiz.id}>
          <Card>
            <CardHeader>{quiz.name}</CardHeader>
          </Card>
        </ReactRouterLink>
      ))}
    </SimpleGrid>
  );
}

export default Home;
