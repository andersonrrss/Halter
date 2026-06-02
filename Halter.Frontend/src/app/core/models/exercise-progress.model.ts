import { ExerciseType } from "./enums/exercise-type.enum";

export interface ExerciseProgressResponse {
  personalRecord?: ExerciseHistoryEntryResponse;
  history?: ExerciseHistoryEntryResponse[] | null;
}

export interface ExerciseHistoryEntryResponse {
  /** @format uuid */
  id?: string;
  exerciseType?: ExerciseType;
  /** @format date-time */
  completedAt?: string;
  /** @format int32 */
  valueAchieved?: number;
  /** @format int32 */
  setsCompleted?: number;
  /** @format double */
  maxWeight?: number | null;
}