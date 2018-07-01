import { BoardItemType } from './board-item-type.enum';

export interface BoardItem {
  id: string;
  title: string;
  content: string;
  type: BoardItemType;
}

