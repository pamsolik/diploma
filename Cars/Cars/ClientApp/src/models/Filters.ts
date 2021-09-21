import {SortOrder} from "./enums/SortOrder";

export interface Filters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  category: string,
  levels: any,
  sortOrder: number
  //TODO: More filters and sort order

}

export function newFilters(): Filters {
  return {
    pageSize: 2,
    pageIndex: 1,
    searchString: "",
    category: "",
    levels: [],
    sortOrder: 1
  }
}
