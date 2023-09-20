import uniqId from "uniqid";

const questionTypes = ["singleChoiceQuestion"];

export function Question(
  questionText = "",
  questionType = "singleChoiceQuestion"
) {
  if (!questionTypes.includes(questionType)) {
    questionType = questionTypes[0];
  }
  const obj = { questionText, questionType, id: uniqId(), parameters: {} };
  switch (obj.questionType) {
    case "singleChoiceQuestion":
      obj.parameters = {
        options: [],
        correctOptionIndex: 0,
      };
      break;
  }
  return obj;
}
