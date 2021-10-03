import {SortOrder} from "./enums/SortOrder";

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
  city: string,
  latitude: number,
  longitude: number
}

export function newFilters(): Filters {
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

    city: "",
    distance: 0,
    latitude: 0,
    longitude: 0
  }
}
