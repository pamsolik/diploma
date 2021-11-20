import {City} from "./City";
import {ApplicationDto} from "./ApplicationDto";

export interface Filters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  category: string,
  levels: any,
  sortOrder: number,
  jobTypes: [],
  jobLevels: [],
  teamSizes: [],
  //From external library
  distance: number,
  city: City
}

export function FiltersDefault(): Filters {
  return {
    pageSize: 5,
    pageIndex: 0,
    searchString: "",
    category: "",
    levels: [],
    sortOrder: 1,

    jobLevels: [],
    jobTypes: [],
    teamSizes: [],

    distance: 0,
    city: new City(),
  }
}
