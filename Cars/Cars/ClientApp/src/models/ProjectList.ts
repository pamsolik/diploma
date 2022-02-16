import { ProjectDto } from "./ProjectDto";

export interface ProjectList {
  items: ProjectDto[],
  totalItems: number,
  pageIndex: number,
  pageSize: number
}


