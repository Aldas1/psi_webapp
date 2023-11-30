import { useQuery } from "@tanstack/react-query";
import { getFlashcardCollections } from "../api/flashcardCollections";
import { Card, CardHeader, SimpleGrid, Spinner } from "@chakra-ui/react";
import { Link as ReactRouterLink } from "react-router-dom";

export default function FlashardCollections() {
  const collectionsQuery = useQuery({
    queryKey: ["flashcard-collections"],
    queryFn: getFlashcardCollections,
  });

  if (collectionsQuery.isLoading) return <Spinner />;
  if (collectionsQuery.isError) return "Unexpected error!";

  const flashcardCollections = collectionsQuery.data;

  return (
    <SimpleGrid gap="2" templateColumns="repeat(auto-fit, minmax(240px, 1fr))">
      {flashcardCollections.map((c) => (
        <ReactRouterLink to={`${c.id}`} key={c.id}>
          <Card>
            <CardHeader>{c.name}</CardHeader>
          </Card>
        </ReactRouterLink>
      ))}
    </SimpleGrid>
  );
}
